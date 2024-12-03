class PostView {
    constructor(id: number, authorId: number, title: string, content: string, createdAt: Date, likes: number) {
        this.id = id;
        this.authorId = authorId;
        this.title = title;
        this.content = content;
        this.createdAt = createdAt;
        this.likes = likes;
    }

    id: number;
    authorId: number;
    title: string;
    content: string;
    createdAt: Date;
    likes: number;
}

export default PostView;