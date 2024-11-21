import { useEffect, useState } from "react";
import Post from "../Models/Post";

function home() {
    const [posts, setPosts] = useState<Post[]>();

    async function populatePosts() {
        await fetch("/api/Post/list",
        {
            credentials: "include"
        })
        .then(async (response) => {
            // ReSharper disable once TsResolvedFromInaccessibleModule
            setPosts(await response.json());
        })
        .catch((error) => {
            console.log(error);
        });
    }

    useEffect(() => {
        populatePosts();
    }, []);

    if (posts === undefined || posts === null) {
        return <p>Loading...</p>;
    }

    return (
        <div className="d-flex flex-column gap-3">
            {posts.map(post =>
                <div key={post.id} className="card overflow-hidden">
                    <div className="card-body">
                        <h5 className="card-title">{post.title}</h5>
                        <p className="card-text">{post.content}</p>
                    </div>
                    <div className="card-footer d-flex justify-content-between">
                        <div></div>
                        <p>{post.author?.username ?? "User not found"}</p>
                    </div>
                </div>
            )}
        </div>
    );
}

export default home;