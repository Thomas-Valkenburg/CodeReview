import { convertFromRaw, Editor, EditorState, RawDraftContentState, RichUtils } from "draft-js";
import "draft-js/dist/Draft.css";
import { useState } from "react";
import "../../css/CommentEditor.css";

function RichTextEditor(props: any) {
    const [editorState, setEditorState] = useState<EditorState>(
        () => props.content === undefined ? EditorState.createEmpty() : EditorState.createWithContent(convertFromRaw(JSON.parse(props.content) as RawDraftContentState)),
    );

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

    function toggleInline(e: React.MouseEvent<HTMLSpanElement>) {
        e.preventDefault();
        setEditorState(RichUtils.toggleInlineStyle(editorState, e.target.title.toUpperCase()));
    }

    function toggleBlock(e: React.MouseEvent<HTMLSpanElement>) {
        e.preventDefault();
        setEditorState(RichUtils.toggleBlockType(editorState, e.target.title));
    }

    function undo(e: React.MouseEvent<HTMLSpanElement>) {
        e.preventDefault();
        setEditorState(EditorState.undo(editorState));
    }

    function redo(e: React.MouseEvent<HTMLSpanElement>) {
        e.preventDefault();
        setEditorState(EditorState.redo(editorState));
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
        <div>
            <div className="d-flex flex-row gap-0 gap-sm-1 gap-md-3">
                <div className="d-flex flex-row">
                    <span className="btn btn-light rounded-0 bi bi-type-bold p-2" title="bold" onMouseDown={toggleInline}></span>
                    <span className="btn btn-light rounded-0 bi bi-type-italic p-2" title="italic" onMouseDown={toggleInline}></span>
                    <span className="btn btn-light rounded-0 bi bi-type-underline p-2" title="underline" onMouseDown={toggleInline}></span>
                </div>
                <div className="d-flex flex-row">
                    <span className="btn btn-light rounded-0 bi bi-braces p-2" title="code-block" onMouseDown={toggleBlock}></span>
                    <span className="btn btn-light rounded-0 bi bi-quote p-2" title="blockquote" onMouseDown={toggleBlock}></span>
                </div>
                <div className="d-flex flex-row">
                    <span className="btn btn-light rounded-0 bi bi-list-ol p-2" title="ordered-list-item" onMouseDown={toggleBlock} />
                    <span className="btn btn-light rounded-0 bi bi-list-ul p-2" title="unordered-list-item" onMouseDown={toggleBlock} />
                </div>
                <div className="d-flex flex-row">
                    <span className="btn btn-light rounded-0 bi bi-arrow-counterclockwise" title="undo" onMouseDown={undo} />
                    <span className="btn btn-light rounded-0 bi bi-arrow-clockwise" onMouseDown={redo} />
                </div>
            </div>
            <hr className="m-0"></hr>
            <div className="p-2">
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
    );
}

export default RichTextEditor;