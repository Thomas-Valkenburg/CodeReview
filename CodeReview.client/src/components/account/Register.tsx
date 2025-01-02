import { useState } from "react";

function register() {
    const [formData, setFormData] = useState({
        email: "",
        password: ""
    });

    const [errors, setErrors] = useState({});

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };

    const validateForm = (formData: any) : boolean => {
        const newErrors = {};

        if (!formData.email) {
            newErrors.email = "Email is required";
        } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
            newErrors.email = "Input is not a valid email";
        }

        const passwordChars: string[] = Array.from(formData.password);

        if (!formData.password) {
            newErrors.password = "Password is required";
        } else if (!passwordChars.some(char => /[A-Z]/.test(char))) {
            newErrors.password = "Password must contain at least one uppercase character";
        } else if (!passwordChars.some(char => /[a-z]/.test(char))) {
            newErrors.password = "Password must contain at least one lowercase character";
        } else if (formData.password.length < 8) {
            newErrors.password = "Password must be at least 8 characters long";
        } else if (formData.password.length > 16) {
            newErrors.password = "Password must be less then 16 characters long";
        }

        setErrors(newErrors);

        if (newErrors.email !== undefined || newErrors.password !== undefined) return false;

        return true;
    }

    const submit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const result = validateForm(formData);

        if (!result) return;

        await fetch("/register", {
            method: "post",
            credentials: "include",
            headers: { 'Content-Type': "application/json" },
            body: JSON.stringify(formData)
        })
        .then((response) => {
            // ReSharper disable once TsResolvedFromInaccessibleModule
            if (response.ok) {
                window.location.href = "/";
                return;
            };

            // ReSharper disable once TsResolvedFromInaccessibleModule
            console.log(response.statusText);
        })
        .catch((error) => {
            console.log(error);
        });
    }

    return (
        <form className="d-flex flex-column m-auto gap-3" noValidate onSubmit={submit}>
            {/*<div>
                 <label htmlFor="username" className="form-label">Username</label>
                 <input type="text" className="form-control"></input>
             </div>*/}
            <div>
                <label htmlFor="email" className="form-label">Email</label>
                <input type="email" name="email" value={formData.email} className="form-control" onChange={handleChange}></input>
                {errors.email && (
                    <span className="text-danger">{errors.email}</span>
                )}
            </div>
            <div>
                <label htmlFor="password" className="form-label">Password</label>
                <input type="password" name="password" value={formData.password} className="form-control" onChange={handleChange}></input>
                {errors.password && (
                    <span className="text-danger">{errors.password}</span>
                )}
            </div>
            <input type="submit" value="Register" className="btn btn-primary rounded-5" />
        </form >
    );
}

export default register;