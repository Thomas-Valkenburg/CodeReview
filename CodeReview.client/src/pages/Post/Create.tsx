import { useEffect } from "react";
import PostForm from "../../components/Editors/PostForm";

function create() {
    async function populateAccountInformation() {
        await fetch("/manage/info", {
            credentials: "include"
        })
        .then(async (response) => {
            // ReSharper disable once TsResolvedFromInaccessibleModule
            if (response.status === 401) {
                window.location.href = escape("/account/login");
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