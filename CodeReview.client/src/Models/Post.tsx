import user from "./User";

interface post {
    id: number;
    author?: user;
    title: string;
    content: string;
}

export default post;