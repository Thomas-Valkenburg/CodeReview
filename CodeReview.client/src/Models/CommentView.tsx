class CommentView {
    constructor(id: number, authorId: number, authorUsername: string, postId: number, content: string) {
        this.id = id;
        this.authorId = authorId;
        this.authorUsername = authorUsername;
        this.postId = postId;
        this.content = content;
    }

    id: number;
    authorId: number;
    authorUsername: string;
    postId: number;
    content: string;
    likes: number;
}

export default CommentView;