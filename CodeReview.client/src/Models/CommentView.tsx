class CommentView {
    constructor(id: number, authorId: number, postId: number, content: string) {
        this.id = id;
        this.authorId = authorId;
        this.postId = postId;
        this.content = content;
    }

    id: number;
    authorId: number;
    postId: number;
    content: string;
}

export default CommentView;