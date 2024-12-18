class PostView {
    constructor(id: number, authorId: number, authorUsername: string, title: string, content: string, comments: number[], createdAt: Date, likes: number) {
        this.id = id;
        this.authorId = authorId;
        this.authorUsername = authorUsername;
        this.title = title;
        this.content = content;
        this.comments = comments;
        this.createdAt = createdAt;
        this.likes = likes;
    }

    id: number;
    authorId: number;
    authorUsername;
    title: string;
    content: string;
    comments: number[];
    createdAt: Date;
    likes: number;
}

export default PostView;