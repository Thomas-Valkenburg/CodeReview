import { useState, useEffect } from "react"
import { useParams } from "react-router-dom"
import PostView from "../../Models/PostView"
import User from "../../Models/User"
import CopyToClipboard from "../../components/CopyToClipboard"

function id() {
    const { postId = 0 } = useParams() as { postId: Number };

    const [post, setPost] = useState<PostView>();
    const [user, setUser] = useState<User>();

    async function getUser() {
        await fetch(`/api/user${post.authorId}`, {
            credentials: "include"
        })
        .then(async (response) => {
            // ReSharper disable once TsResolvedFromInaccessibleModule
            setUser(await response.json());
        })
        .catch((error) => {
            console.log(error);
        });
    }

    async function populatePosts() {
        await fetch(`/api/Post?id=${postId}`, {
            credentials: "include"
        })
        .then(async (response) => {
            // ReSharper disable once TsResolvedFromInaccessibleModule
            setPost(await response.json());

            getUser();
        })
        .catch((error) => {
            console.log(error);
        });
    }

    useEffect(() => {
        populatePosts();
    },
    []);

    if (post === undefined || post === null) {
        return <p>Loading...</p>;
    }

    return (
        <div className="d-flex flex-column">
            <h2>{post.title}</h2>
            <p>{post.content}</p>
            <div className="d-flex flex-row justify-content-between">
                <div>
                    <button id="share" className="btn btn-light" onClick={() => CopyToClipboard(window.location.href)}>Share</button>
                </div>
                <a className="btn" href={`/user/${post.authorId}`}>{user.username}</a>
            </div>
        </div>
    );
}

export default id;