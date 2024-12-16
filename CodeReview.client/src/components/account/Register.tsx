import { useState } from "react";

function register() {
    const [errorMessage, setErrorMessage] = useState<String>();

    const submit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const formData = new FormData(e.currentTarget);
        const data: any = {};
        formData.forEach((value, key) => data[key] = value);

        await fetch("/register", {
            method: "post",
            credentials: "include",
            headers: { 'Content-Type': "application/json" },
            body: JSON.stringify(data)
        })
        .then((response) => {
            // ReSharper disable once TsResolvedFromInaccessibleModule
            if (response.ok) {
                window.location.href = "/";
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
        <form className="d-flex flex-column m-auto gap-3" onSubmit={submit}>
            {errorMessage == null ? "" : <div className="m-auto alert alert-danger">{errorMessage}</div>}
            <div>
                <label htmlFor="username" className="form-label">Username</label>
                <input type="text" className="form-control"></input>
            </div>
            <div>
                <label htmlFor="email" className="form-label">Email</label>
                <input type="email" name="email" className="form-control" required></input>
            </div>
            <div>
                <label htmlFor="password" className="form-label">Password</label>
                <input type="password" name="password" className="form-control" required></input>
            </div>
            <input type="submit" value="Register" className="btn btn-primary rounded-5" />
        </form >
    );
}

export default register;