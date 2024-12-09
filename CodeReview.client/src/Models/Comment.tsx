import user from "./User";
import post from "./Post";

interface IComment {
    id: number;
    author: user;
    post: post;
    content: string;
}

export default IComment;