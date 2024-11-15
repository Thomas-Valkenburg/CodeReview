import user from "./User";
import post from "./Post";

interface comment {
    id: number;
    author: user;
    post: post;
    content: string;
}

export default comment;