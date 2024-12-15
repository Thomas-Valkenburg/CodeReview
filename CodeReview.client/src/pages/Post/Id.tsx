import { useState, useEffect } from "react"
import { useParams } from "react-router-dom"
import PostView from "../../Models/PostView"
import User from "../../Models/User"
import Comment from "../../Models/CommentView"
import CopyToClipboard from "../../scripts/CopyToClipboard"

function id() {
    const { postId = 0 } = useParams();

    const [post, setPost] = useState<PostView>();
    const [user, setUser] = useState<User>();
    const [comments, setComments] = useState<Comment[]>();

    async function populatePost() {
        await fetch(`/api/Post?id=${postId}`, {
            credentials: "include"
        }).then(async (response) => {
            // ReSharper disable once TsResolvedFromInaccessibleModule
            const data = await response.json() as PostView;
            setPost(data);
            populateUser(data.authorId);
            populateComments(data.comments);
        }).catch((error) => {
            console.log(error);
        });
    }

    async function populateUser(authorId: number) {
        await fetch(`/api/user/${authorId}`, {
            credentials: "include"
        }).then(async (response) => {
            // ReSharper disable once TsResolvedFromInaccessibleModule
            const data = await response.json();
            setUser(data);
        }).catch((error) => {
            console.log(`Error: ${error}`);
        });
    };

    async function populateComments(commentIds: number[]) {

        if (commentIds === undefined || commentIds === null || commentIds.length < 1) return;

        const comments: Comment[] = [];

        for (let i = 0; i < commentIds.length; i++) {
            await fetch(`/api/comment/${commentIds[i]}`, {
                credentials: "include"
            }).then(async (response) => {
                // ReSharper disable once TsResolvedFromInaccessibleModule
                comments.push(await response.json() as Comment);
            }).catch((error) => {
                console.timeLog(`Error: ${error}`);
            });
        }

        setComments(comments);
    };

    useEffect(() => {
        populatePost();
    }, []);

    if (post === undefined || post === null || user === undefined || user === null || comments === undefined || comments === null) {
        return <p>Loading...</p>;
    }

    return (
        <div className="d-flex flex-column px-md-5 py-md-2 bg-light">
            <h2>{post.title}</h2>
            <p>{post.content}</p>
            <hr></hr>
            <div className="d-flex flex-row justify-content-between">
                <div>
                    <button id="share" className="btn btn-light" onClick={() => CopyToClipboard(window.location.href)}>Share</button>
                </div>
                <a className="btn" href={`/user/${post.authorId}`}>{user.username}</a>
            </div>
            <div>
                {comments.map(comment =>
                    <div className="card">
                        <div className="card-body">{comment.content}</div>
                        <a href={`/user/${comment.authorId}`} className="card-footer">User's profile</a>
                    </div>
                )}
            </div>
        </div>
    );
}

export default id;