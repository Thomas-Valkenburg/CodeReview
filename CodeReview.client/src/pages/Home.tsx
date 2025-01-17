import { useEffect, useState } from "react";
import Editor from "../components/Editors/CommentEditor";
import PostView from "../Models/PostView";

function home() {
    const [posts, setPosts] = useState<PostView[]>();

    async function populatePosts() {
        const search = new URLSearchParams(window.location.search).get("search");

        await fetch(`/api/post/list${search === null ? "" : `?search=${search}`}`,
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
            <div className="d-flex justify-content-between">
                <div></div>
                <div>
                    <a id="create" href="/post/create" className="btn btn-success">Ask a question</a>
                </div>
            </div>
            {posts.length > 0 ? posts.map(post =>
                    <a key={post.id} href={`/post/${post.id}`} className="card overflow-hidden text-decoration-none">
                        <div className="card-body">
                            <h5 className="card-title">{post.title}</h5>
                            <div className="card-text overflow-hidden" style={{ "maxHeight": "150px" }}>
                                <Editor content={post.content} readOnly={true} />
                            </div>
                        </div>
                        <div className="card-footer d-flex justify-content-between">
                            <div></div>
                            <p>{post.authorUsername ?? "User not found"}</p>
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