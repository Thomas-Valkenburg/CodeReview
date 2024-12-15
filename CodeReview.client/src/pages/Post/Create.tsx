import { useState } from "react"

function create() {
    const [errorMessage, setErrorMessage] = useState<String>();

    const submit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const formData = new FormData(e.currentTarget);
        const data: object = {};
        formData.forEach((value, key) => data[key] = value);

        await fetch("api/post/create", {
            method: "post",
            credentials: "include",
            headers: { 'Content-Type': "application/json" },
            body: JSON.stringify(data)
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

    return (
        <form className="d-flex flex-column m-auto gap-3 mt-2 col-11" onSubmit={submit}>
            {errorMessage == null ? "" : <div className="m-auto alert alert-danger">{errorMessage}</div>}
            <input type="number" name="id" className="form-control" hidden></input>
            <label htmlFor="title" className="form-label">Title</label>
            <input type="text" name="title" className="form-control" required></input>
            <label htmlFor="content">Description</label>
            <textarea name="content" className="form-control" rows="20" required></textarea>
            <div className="d-flex flex-column col-3 align-items-center w-100">
                <input type="submit" value="Create" className="btn btn-outline-success px-4" />
            </div>
        </form>
    );
}

export default create;