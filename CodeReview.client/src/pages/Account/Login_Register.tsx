import { useState } from "react"
import { useParams } from "react-router-dom"
import Login from "./Login"
import Register from "./Register"

function Login_Register() {
    const [login, setLogin] = useState(window.location.pathname === "/account/login");

    console.log(window.location.pathname);

    function SelectLogin(event: React.FormEventHandler<HTMLDivElement>) {
        setLogin(event.target.defaultValue === "true");
    }

    return (
        <div className="d-flex flex-column align-items-stretch gap-3 mt-3 m-auto" style={{ maxWidth: "750px" }}>
            <div className="d-flex gap-2 bg-light rounded-5 p-2 m-auto" onChange={SelectLogin}>
                <input type="radio" className="btn-check" name="option" value="true" id="login" defaultChecked={login}></input>
                <label className="btn btn-outline-primary rounded-5" htmlFor="login">Login</label>
                <input type="radio" className="btn-check" name="option" value="false" id="register" defaultChecked={!login}></input>
                <label className="btn btn-outline-primary rounded-5" htmlFor="register">Register</label>
            </div>
            <div className="bg-light rounded-5 px-4 px-md-5 py-5">
                { login ? <Login/> : <Register/>}
            </div>
        </div>
    );
}

export default Login_Register;