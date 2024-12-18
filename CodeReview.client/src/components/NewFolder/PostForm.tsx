import { convertFromRaw, convertToRaw, Editor, EditorState, RawDraftContentState, RichUtils, Modifier, SelectionState } from "draft-js";
import { useState } from "react";
import { useParams } from "react-router-dom";

function PostForm(props: any) {
    const params = useParams();

    const [errorMessage, setErrorMessage] = useState<string>();
    const [editorState, setEditorState] = useState<EditorState>(
        () => props.content === undefined ? EditorState.createEmpty() : EditorState.createWithContent(convertFromRaw(JSON.parse(props.content) as RawDraftContentState)),
    );

    const submit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        await fetch(`/api/post?title=${e.currentTarget.title.value}&editorContent=${JSON.stringify(convertToRaw(editorState.getCurrentContent()))}`, {
            method: "POST",
            credentials: "include"
        })
        .then(async (response) => {
            // ReSharper disable once TsResolvedFromInaccessibleModule
            if (response.ok) {
                // ReSharper disable once TsResolvedFromInaccessibleModule
                const responseData = await response.json();

                window.location.href = `/post/${responseData.id}`;
                return;
            };

            // ReSharper disable once TsResolvedFromInaccessibleModule
            setErrorMessage(response.statusText);
        })
        .catch((error) => {
            setErrorMessage(error);
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

    function toggleInline(e: React.MouseEvent<HTMLSpanElement>) {
        e.preventDefault();
        setEditorState(RichUtils.toggleInlineStyle(editorState, e.target.title.toUpperCase()));
    }

    function toggleBlock(e: React.MouseEvent<HTMLButtonElement>) {
        e.preventDefault();
        setEditorState(RichUtils.toggleBlockType(editorState, e.target.title));
    }

    function undo(e: React.MouseEvent<HTMLButtonElement>) {
        e.preventDefault();
        setEditorState(EditorState.undo(editorState));
    }

    function redo(e: React.MouseEvent<HTMLButtonElement>) {
        e.preventDefault();
        setEditorState(EditorState.redo(editorState));
    }

    return (
        <form className="d-flex flex-column m-auto gap-3 mt-2 col-11" onSubmit={submit}>
            {errorMessage == null ? "" : <div className="m-auto alert alert-danger">{errorMessage}</div>}
            <label htmlFor="title" className="form-label">Title</label>
            <input type="text" name="title" className="form-control" required></input>
            <label htmlFor="content">Description</label>
            <div className="d-flex flex-column gap-3">
                <div className="border border-1 rounded-2 focus-within-ring">
                    <div className="d-flex flex-row gap-0 gap-sm-1 gap-md-3">
                        <div className="d-flex flex-row">
                            <span className="btn btn-light bi bi-type-bold p-2" title="bold" onMouseDown={toggleInline}></span>
                            <span className="btn btn-light bi bi-type-italic p-2" title="italic" onMouseDown={toggleInline}></span>
                            <span className="btn btn-light bi bi-type-underline p-2" title="underline" onMouseDown={toggleInline}></span>
                        </div>
                        <div className="d-flex flex-row">
                            <span className="btn btn-light bi bi-braces p-2" title="code-block" onMouseDown={toggleBlock}></span>
                            <span className="btn btn-light bi bi-quote p-2" title="blockquote" onMouseDown={toggleBlock}></span>
                        </div>
                        <div className="d-flex flex-row">
                            <span className="btn btn-light bi bi-list-ol p-2" title="ordered-list-item" onMouseDown={toggleBlock} />
                            <span className="btn btn-light bi bi-list-ul p-2" title="unordered-list-item" onMouseDown={toggleBlock} />
                        </div>
                        <div>
                            <span className="btn btn-light bi bi-arrow-counterclockwise" onMouseDown={undo} />
                            <span className="btn btn-light bi bi-arrow-clockwise" onMouseDown={redo} />
                        </div>
                    </div>
                    <hr className="m-0"></hr>
                    <div className="p-2 overflow-hidden">
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
            </div>
            <button type="submit" className="btn btn-primary align-self-center px-4">Create</button>
        </form>
    );
}

export default PostForm;