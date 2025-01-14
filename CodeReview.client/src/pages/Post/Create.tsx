import { useState, useEffect } from "react"
import PostForm from "../../components/NewFolder/PostForm";

function create() {
    const [email, setEmail] = useState<String>();

    async function populateAccountInformation() {
        await fetch("/manage/info", {
            credentials: "include"
        })
        .then(async (response) => {
            // ReSharper disable once TsResolvedFromInaccessibleModule
            if (response.status === 401) {
                window.location.href = "/account/login";
            }
        })
        .catch((error) => console.log(error));
    }

    useEffect(() => {
        populateAccountInformation();
    }, []);

    return (
        <div>
            <PostForm />
        </div>
    );
}

export default create;