function login() {

    const submit = async (email: string, password: string) => {
        const requestOptions = {
            method: "POST",
            headers: { 'Content-Type': "application/json" },
            body: JSON.stringify({ email: email, password: password })
        }

        const response = await fetch("/login", requestOptions);
    }

    return (
        <form className="d-flex flex-column m-auto gap-3 mt-2 col-5">
            <label htmlFor="email" className="form-label">Email</label>
            <input type="email" name="email" className="form-control" required></input>
            <label htmlFor="password">Password</label>
            <input type="password" name="password" className="form-control" required></input>
            <div className="d-flex flex-column col-3 align-items-center w-100">
                <input type="submit" className="btn btn-outline-success" onClick={submit} />
            </div>
        </form>
    );
}

export default login;