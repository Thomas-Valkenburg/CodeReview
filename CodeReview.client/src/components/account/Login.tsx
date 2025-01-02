import { useState } from "react";

function login() {
    const [errorMessage, setErrorMessage] = useState<String>();

    const submit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const formData = new FormData(e.currentTarget);
        const data: any = {};
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
            if (response.status === 401) {
                setErrorMessage("Invalid email or password.");
                return;
            }

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
        <form className="d-flex flex-column gap-3" onSubmit={submit}>
            {errorMessage == null ? "" : <p className="m-auto alert alert-danger">{errorMessage}</p>}
            <div>
                <label htmlFor="email" className="form-label">Email</label>
                <input type="email" name="email" className="form-control" required></input>
            </div>
            <div>
                <label htmlFor="password" className="form-label">Password</label>
                <input type="password" name="password" className="form-control" required></input>
            </div>
            <input type="submit" value="Log in" className="btn btn-primary rounded-5" />
        </form >
    );
}

export default login;
