import { useEffect, useState } from "react";
import Post from "../Models/Post";

function Home() {
    const [posts, setPosts] = useState<Post[]>();

    async function populatePosts() {
        const response = await fetch("/api/Post/list");
// ReSharper disable once TsResolvedFromInaccessibleModule
        const data = await response.json();
        console.log(data);
        setPosts(data);
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

export default Home;