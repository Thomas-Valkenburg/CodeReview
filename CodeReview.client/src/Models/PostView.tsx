class PostView {
    constructor(id: number, authorId: number, title: string, content: string, comments: number[], createdAt: Date, likes: number) {
        this.id = id;
        this.authorId = authorId;
        this.title = title;
        this.content = content;
        this.comments = comments;
        this.createdAt = createdAt;
        this.likes = likes;
    }

    id: number;
    authorId: number;
    title: string;
    content: string;
    comments: number[];
    createdAt: Date;
    likes: number;
}

export default PostView;