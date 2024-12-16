import { useState } from "react"
import Login from "../../components/account/Login.tsx"
import Register from "../../components/account/Register"

function loginRegister() {
    const [login, setLogin] = useState(window.location.pathname === "/account/login");

    function selectLogin(event: React.FormEvent<HTMLDivElement>) {
        setLogin((event.target as HTMLInputElement).defaultValue === "true");
    }

    const welcomeMessage = login ? "Good to see you again." : "Pleased to meet you.";

    return (
        <div className="d-flex flex-column align-items-stretch bg-light rounded-5 mt-3 mt-md-5 p-4 m-auto" style={{ maxWidth: "750px" }}>
            <div className="d-flex gap-1 bg-light rounded-5 m-auto" onChange={selectLogin}>
                <input type="radio" className="btn-check" name="option" value="true" id="login" defaultChecked={login}></input>
                <label className="btn-bottom btn-outline-primary border-primary border-4 rounded-0 pb-3 px-3 mb-0" htmlFor="login">Login</label>
                <input type="radio" className="btn-check" name="option" value="false" id="register" defaultChecked={!login}></input>
                <label className="btn-bottom btn-outline-primary border-primary border-4 rounded-0 pb-3 px-3 mb-0" htmlFor="register">Register</label>
            </div>
            <hr className="mt-0"></hr>
            <div className="d-flex flex-row justify-content-center bg-light rounded-5 p-2">
                <div className="flex-grow-1">
                    {login ? <Login /> : <Register />}
                </div>
                <div className="d-none d-sm-flex flex-column align-items-center col-6 px-4 text-center">
                    <h1 className={"bi" + (login ? " bi-patch-check" : " bi-door-open")}></h1>
                    <h1 style={{ maxWidth: "200px" }}>
                        {welcomeMessage}
                    </h1>
                </div>
            </div>
        </div>
    );
}

export default loginRegister;