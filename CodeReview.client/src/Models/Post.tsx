import user from "./User";

interface IPost {
    id: number;
    author: user;
    title: string;
    createdAt: Date;
    content: string;
    likes: number;
}

export default IPost;