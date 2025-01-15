import post from "./Post";
import user from "./User";

interface IComment {
    id: number;
    author: user;
    post: post;
    content: string;
}

export default IComment;