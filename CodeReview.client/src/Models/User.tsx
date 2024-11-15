import post from "./Post";
import comment from "./Comment";

interface user {
    id: number;
    username: string;
    posts: post[];
    comments: comment[];
}

export default user;