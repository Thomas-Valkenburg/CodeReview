import { useState, useEffect } from "react"
import { useParams } from "react-router-dom"
import PostView from "../../Models/PostView"
import User from "../../Models/User"
import Comment from "../../Models/CommentView"
import CommentEditor from "../../components/CommentEditor"
import CopyToClipboard from "../../scripts/CopyToClipboard"

function id() {
    const params = useParams();

    const [post, setPost] = useState<PostView>();
    const [user, setUser] = useState<User>();
    const [comments, setComments] = useState<Comment[]>();

    async function populatePost() {
        await fetch(`/api/Post/${params["id"]}`, {
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

    if (post === undefined || post === null || user === undefined || user === null) {
        return <p className="p-3 m-auto text-center">Loading...</p>;
    }

    return (
        <div className="d-flex flex-column gap-4">
            <div className="rounded-4 px-3 py-2 px-md-4 py-md-3 mt-3 mt-md-4">
                <h2>{post.title}</h2>
                <p>{post.content}</p>
                <hr></hr>
                <div className="d-flex flex-row justify-content-between">
                    <div>
                        <button id="share" className="btn btn-light" onClick={() => CopyToClipboard(window.location.href)}>Share</button>
                    </div>
                    <a className="btn btn-light" href={`/user/${post.authorId}`}>{user.username}</a>
                </div>
            </div>
            <div>
                {comments === undefined || comments === null ? 
                    <h4 className="px-3 py-2 px-md-4 py-md-3 mt-3 mt-md-4">No answers yet.</h4>
                    :
                    <div>
                        <h4 className="px-3 py-2 px-md-4 py-md-3 mt-3 mt-md-4">{comments.length} Answers:</h4>
                        {
                            comments.map(comment =>
                                <div className="px-3 py-2 px-md-4 py-md-3 mt-3 mt-md-4">
                                    <CommentEditor content={comment.content} readOnly={true}></CommentEditor>
                                    <hr></hr>
                                </div>
                            )
                        }
                    </div>
                }
            </div>
            <div className="rounded-4 px-3 py-2 px-md-4 py-md-3 mt-3 mt-md-4">
                <h4>Your answer:</h4>
                <CommentEditor></CommentEditor>
            </div>
        </div>
    );
}

export default id;