import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Comment from "../../Models/CommentView";
import PostView from "../../Models/PostView";
import CommentEditor from "../../components/Editors/CommentEditor";
import CopyToClipboard from "../../scripts/CopyToClipboard";

function id() {
    const params = useParams();

    const [post, setPost] = useState<PostView>();
    const [comments, setComments] = useState<Comment[]>();

    async function populatePost() {
        await fetch(`/api/Post/${params["id"]}`, {
            credentials: "include"
        }).then(async (response) => {
            // ReSharper disable once TsResolvedFromInaccessibleModule
            const data = await response.json() as PostView;
            setPost(data);
            populateComments(data.comments);
        }).catch((error) => {
            console.log(error);
        });
    }

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

        comments.sort((a, b) => {
            return a.likes - b.likes;
        });

        setComments(comments);
    };

    async function postLike(e: React.MouseEvent<HTMLButtonElement>) {
        e.preventDefault();

        const scrollPosition = window.scrollY;

        window.scrollTo(null, scrollPosition - 50);
    }

    useEffect(() => {
        populatePost();
    }, []);

    if (post === undefined || post === null) {
        return <p className="p-3 m-auto text-center">Loading...</p>;
    }

    return (
        <div className="d-flex flex-column gap-4">
            <div className="d-flex flex-row gap-3 px-3 py-2 px-md-4 py-md-3">
                <div>
                    <div className="d-flex flex-column align-items-center">
                        <button className="btn bi bi-arrow-up-circle p-0" style={{ fontSize: "1.5rem" }} onMouseDown={postLike}></button>
                        <span className="p-0" style={{ fontSize: "1.25rem" }}>{post.likes}</span>
                    </div>
                </div>
                <div className="d-flex flex-column flex-grow-1">
                    <h2>{post.title}</h2>
                    <div className="flex-grow-1 overflow-hidden">
                        <CommentEditor content={post.content} readOnly={true}>{post.content}</CommentEditor>
                    </div>
                    <div className="d-flex flex-row justify-content-between">
                        <div>
                            <button className="btn btn-outline-primary" onMouseDown={() => CopyToClipboard(window.location.host + window.location.pathname)}>Share</button>
                        </div>
                        <a className="d-none d-sm-block btn btn-outline-primary" href={`/user/${post.authorId}`}>{post.authorUsername}</a>
                        <a className="d-sm-none btn btn-outline-primary bi bi-person" href={`/user/${post.authorId}`}></a>
                    </div>
                </div>
            </div>
            <hr></hr>
            {comments === undefined || comments === null ?
                <h4 className="px-3 py-2 px-md-4 py-md-3">No answers yet.</h4>
                :
                <div>
                    <h4 className="px-3 py-2 px-md-4 py-md-3">{comments.length} {comments.length > 1 ? "Answers:" : "Answer"}:</h4>
                    {
                        comments.map(comment =>
                            <div id={`answer-${comment.id}`} key={comment.id} style={{ scrollMarginTop: document.getElementById("navbar").scrollHeight }}>
                                <div className="d-flex gap-3 px-3 py-2 px-md-4 py-md-3">
                                    <div className="d-flex flex-column align-items-center">
                                        <button className="btn bi bi-arrow-up-circle p-0" style={{ fontSize: "1.5rem" }} onMouseDown={postLike}></button>
                                        <span className="p-0" style={{ fontSize: "1.25rem" }}>{comment.likes}</span>
                                    </div>
                                    <div className="d-flex flex-column flex-grow-1">
                                        <div className="flex-grow-1">
                                            <CommentEditor content={comment.content} readOnly={true}></CommentEditor>
                                        </div>
                                        <div className="d-flex flex-row justify-content-between">
                                            <div>
                                                <button className="btn btn-outline-primary" onClick={() => CopyToClipboard(window.location.host + window.location.pathname + `#answer-${comment.id}`)}>Share</button>
                                            </div>
                                            <div>
                                                <a href={`/user/${comment.authorId}`} className="d-none d-sm-block btn btn-outline-primary">{comment.authorUsername}</a>
                                                <a href={`/user/${comment.authorId}`} className="d-sm-none btn btn-outline-primary bi bi-person"></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr></hr>
                            </div>
                        )
                    }
                </div>
            }
            <div className="rounded-4 px-3 py-2 px-md-4 py-md-3">
                <h4>Your answer:</h4>
                <CommentEditor></CommentEditor>
            </div>
        </div>
    );
}

export default id;