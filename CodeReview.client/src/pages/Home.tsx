import { useEffect, useState } from "react";
import PostView from "../Models/PostView";

function home() {
    const [posts, setPosts] = useState<PostView[]>();

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
            {posts.length > 0 ? posts.map(post =>
                    <a key={post.id} href={`/post/${post.id}`} className="card overflow-hidden">
                        <div className="card-body">
                            <h5 className="card-title">{post.title}</h5>
                            <p className="card-text">{post.content}</p>
                        </div>
                        <div className="card-footer d-flex justify-content-between">
                            <div></div>
                            <p>{post.authorId ?? "User not found"}</p>
                        </div>
                    </a>
                )
                :
                <p>No posts available...</p>
            }
        </div>
    );
}

export default home;