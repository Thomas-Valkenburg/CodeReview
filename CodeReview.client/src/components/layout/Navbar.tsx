import { useEffect, useState } from "react";

const navbar = () => {
    const [email, setEmail] = useState<String>();
    const [search, setSearch] = useState<boolean>(false);

    async function populateAccountInformation() {
        await fetch("/manage/info", {
            credentials: "include"
        })
        .then(async (response) => {
            // ReSharper disable once TsResolvedFromInaccessibleModule
            //setEmail(response.json());

            const data = await response.json();
            setEmail(data.email);
        })
        .catch((error) => console.log(error));
    }

    function openSearch() {
        setSearch(true);
    }

    function closeSearch() {
        setSearch(false);
    }

    function handleResize() {
        if (window.innerWidth > 575) {
            closeSearch();
        }
    }

    function syncSearchInput(e: React.ChangeEventHandler<HTMLInputElement>) {
        const searchBar = document.getElementById("search-bar") as HTMLInputElement;
        const mobileSearchBar = document.getElementById("mobile-search-bar") as HTMLInputElement;

        searchBar.value = e.target.value;
        mobileSearchBar.value = e.target.value;
    }

    useEffect(() => {
        populateAccountInformation();

        window.addEventListener("resize", handleResize);
    }, []);

    const emailElement = email == undefined
        ?
        <div className="d-flex gap-2">
            <a href="/account/login" className="btn btn-primary px-2">Login</a>
            <a href="/account/register" className="d-none d-md-block btn btn-light px-2">Register</a>
        </div>
        :
        <div>
            <a href="/account/info" className="d-none d-lg-block btn btn-secondary px-2">{email}</a>
            <a href="/account/info" className="d-lg-none btn btn-primary bi bi-person"></a>
        </div>;

    return (
        <header>
            <nav className="navbar border-bottom border-2">
                <div className="container-fluid flex-nowrap">
                    <a className={"navbar-brand" + (search ? " d-none" : "")} href="/#">
                        CodeReview
                    </a>
                    <div className={"bg-white d-flex flex-grow-1" + (search ? "" : " d-none")}>
                        <form role="search" className="flex-grow-1">
                            <div className="d-flex gap-1 form-control py-0">
                                <input type="text" name="search" placeholder="Search" id="mobile-search-bar" className="form-control-plaintext p-0" aria-label="Search" onChange={syncSearchInput}></input>
                                <button type="submit" className="btn bi bi-search"></button>
                            </div>
                        </form>
                        <button className="btn border-0 bi bi-x-lg" onClick={closeSearch}></button>
                    </div>
                    <div className={"d-flex gap-1 justify-content-lg-end" + (search ? " d-none" : "")}>
                        <div className="d-sm-none">
                            <button type="button" aria-expanded="false" className="btn btn-outline-secondary bi bi-search" onClick={openSearch}></button>
                        </div>
                        <form role="search" className="d-none d-sm-block position-absolute center col-sm-6">
                            <div className="d-flex gap-2 form-control py-0">
                                <input type="text" name="search" placeholder="Search" id="search-bar" className="form-control-plaintext" aria-label="Search" onChange={syncSearchInput}></input>
                                <button type="submit" className="btn bi bi-search"></button>
                            </div>
                        </form>
                        {emailElement}
                    </div>
                </div>
            </nav>
        </header>
    );
};

export default navbar