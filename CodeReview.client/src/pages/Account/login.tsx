import { useState } from "react";

function login() {
    const [errorMessage, setErrorMessage] = useState<String>();

    const submit = async (e) => {
        e.preventDefault();

        const formData = new FormData(e.target);
        const data = {};
        formData.forEach((value, key) => data[key] = value);

        await fetch("/login?useCookies=true&useSessionCookies=true", {
            method: "post",
            credentials: "include",
            headers: { 'Content-Type': "application/json" },
            body: JSON.stringify(data)
        })
        .then(async (response) => {
            // ReSharper disable once TsResolvedFromInaccessibleModule
            if (response.ok) {
                window.location.href = "/";
                return;
            };

            // ReSharper disable once TsResolvedFromInaccessibleModule
            const body = await response.json();
            setErrorMessage(body.title);
        })
        .catch((error) => {
            setErrorMessage("Unexpected error.");
            console.log(error);
        });
    }

    return (
        <>
            <form className="d-flex flex-column m-auto gap-3 mt-2 col-5" onSubmit={submit}>
                {errorMessage == null ? "" : <div className="m-auto alert alert-danger">{errorMessage}</div>}
                <label htmlFor="email" className="form-label">Email</label>
                <input type="email" name="email" className="form-control" required></input>
                <label htmlFor="password">Password</label>
                <input type="password" name="password" className="form-control" required></input>
                <div className="d-flex flex-column col-3 align-items-center w-100">
                    <input type="submit" value="Log in" className="btn btn-outline-success"/>
                </div>
            </form >
        </>
    );
}

export default login;