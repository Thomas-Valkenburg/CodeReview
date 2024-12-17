import { useState } from "react"
import { useParams } from "react-router-dom"
import { Editor, EditorState, RichUtils } from "draft-js"
import "draft-js/dist/Draft.css"
import { convertToRaw, convertFromRaw, RawDraftContentState } from "draft-js";

function CommentEditor(props: any) {
    const params = useParams();

    const [editorState, setEditorState] = useState<EditorState>(
        () => props.content === undefined ? EditorState.createEmpty() : EditorState.createWithContent(convertFromRaw(JSON.parse(props.content) as RawDraftContentState)),
    );

    async function postComment() {
        const content = convertToRaw(editorState.getCurrentContent());

        await fetch(`/api/comment?postId=${params["id"]}&content=${JSON.stringify(content)}`, {
            credentials: "include",
            method: "POST",
        });
    }

    function handleKeyCommand(command: Draft.Component.Base.EditorCommand, editorState: EditorState) {
        setEditorState(RichUtils.handleKeyCommand(editorState, command));
    }

    function blockStyle(contentBlock: Draft.Model.ImmutableData.ContentBlock) {
        const type = contentBlock.getType() as string;

        switch (type) {
        case "code-block":
            return "bg-body-secondary rounded-3 p-3";
        case "blockquote":
            return "border-start border-4 border-body-secondary ps-3";
        default:
            return "";
        }
    }

    function toggleInline(e: React.MouseEvent<HTMLButtonElement>) {
        e.preventDefault();
        setEditorState(RichUtils.toggleInlineStyle(editorState, e.target.value));
    }

    function toggleBlock(e: React.MouseEvent<HTMLButtonElement>) {
        e.preventDefault();
        setEditorState(RichUtils.toggleBlockType(editorState, e.target.value));
    }

    return (props.readOnly ?
        <div className="p-2">
            <Editor
                editorState={editorState}
                handleKeyCommand={handleKeyCommand}
                onChange={setEditorState}
                blockStyleFn={blockStyle}
                readOnly
                autoCapitalize="true"
                autoCorrect="true"
                autoComplete="true"
                spellCheck
            />
        </div>
        :
        <div className="d-flex flex-column gap-3">
            <div className="border border-1 rounded-2">
                <div className="d-flex flex-row gap-0 gap-sm-1 gap-md-3">
                    <div className="d-flex flex-row">
                        <button className="btn btn-light bi bi-type-bold p-2" value="BOLD" onMouseDown={toggleInline}></button>
                        <button className="btn btn-light bi bi-type-italic p-2" value="ITALIC" onMouseDown={toggleInline}></button>
                        <button className="btn btn-light bi bi-type-underline p-2" value="UNDERLINE" onMouseDown={toggleInline}></button>
                    </div>
                    <div className="d-flex flex-row">
                        <button className="btn btn-light bi bi-braces p-2" value="code-block" onMouseDown={toggleBlock}></button>
                        <button className="btn btn-light bi bi-quote p-2" value="blockquote" onMouseDown={toggleBlock}></button>
                    </div>
                    <div className="d-flex flex-row">
                        <button className="btn btn-light bi bi-list-ol p-2" value="ordered-list-item" onMouseDown={toggleBlock}></button>
                        <button className="btn btn-light bi bi-list-ul p-2" value="unordered-list-item" onMouseDown={toggleBlock}></button>
                    </div>
                </div>
                <hr className="m-0"></hr>
                <div className="p-2 overflow-hidden" style={{resize: "vertical"}}>
                    <Editor
                        editorState={editorState}
                        handleKeyCommand={handleKeyCommand}
                        onChange={setEditorState}
                        blockStyleFn={blockStyle}
                        autoCapitalize="true"
                        autoCorrect="true"
                        autoComplete="true"
                        spellCheck
                    />
                </div>
            </div>
            <button className="btn btn-primary align-self-center" onClick={postComment}>Add comment</button>
        </div>
    );
}

export default CommentEditor;