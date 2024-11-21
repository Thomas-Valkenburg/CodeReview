import { useEffect, useState } from "react";

const navbar = () => {
    const [email, setEmail] = useState<String>();

    async function PopulateAccountInformation() {
        await fetch("/manage/info", {
            credentials: "include"
        })
        .then(async (response) => {
            // ReSharper disable once TsResolvedFromInaccessibleModule
            //setEmail(response.json());

            const data = await response.json();
            setEmail(data.email);
            console.log(email);
        })
        .catch((error) => console.log(error));
    }

    useEffect(() => {
        PopulateAccountInformation();
    }, []);

    const emailElement = email == undefined
        ?
        <div className="d-flex gap-2">
            <a href="/account/login" className="btn btn-outline-dark py-1 px-2">Login</a>
            <a className="btn btn-outline-dark py-1 px-2">Register</a>
        </div>
        :
        <div>
            <a href="/account/info" className="btn btn-outline-dark py-1 px-2">{email}</a>
        </div>;

    return (
        <>
            <header>
                <nav className="navbar navbar-expand-sm border-bottom border-2">
                    <div className="container">
                        <a className="navbar-brand" href="/">
                            CodeReview
                        </a>
                        <form style={{ 'position': "absolute", 'left': "50%", 'width': "20rem", 'marginLeft': "-10rem" }}>
                            <input type="text" placeholder="Search" className="form-control form-text"></input>
                        </form>
                        {emailElement}
                    </div>
                </nav>
            </header>
        </>
    );
};

export default navbar