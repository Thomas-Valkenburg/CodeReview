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
            const data = await response.json();
            setEmail(data.email);
        })
        .catch((error) => console.log(error));
    }

    async function logout() {
        await fetch(`/logout?returnUrl=${window.location.protocol}//${window.location.host}/`, {
            credentials: "include",
            method: "POST"
        }).then(response => {
            console.log(response);

            // ReSharper disable TsResolvedFromInaccessibleModule
            if (response.ok) {
                if (response.redirected) {
                    window.location.href = response.url;
                }
            }
            // ReSharper restore TsResolvedFromInaccessibleModule
        });
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

    function syncSearchInput(event: React.ChangeEvent<HTMLInputElement>) {
        const searchBar = document.getElementById("search-bar") as HTMLInputElement;
        const mobileSearchBar = document.getElementById("mobile-search-bar") as HTMLInputElement;

        searchBar.value = (event.target as HTMLInputElement).value;
        mobileSearchBar.value = (event.target as HTMLInputElement).value;
    }

    useEffect(() => {
        populateAccountInformation();

        window.addEventListener("resize", handleResize);
    }, []);

    return (
        <header className="sticky-top bg-body" id="navbar">
            <nav className="navbar border-bottom border-2">
                <div className="container-fluid flex-nowrap">
                    <a className={"navbar-brand" + (search ? " d-none" : "")} href="/#">
                        CodeReview
                    </a>
                    <div className={"bg-white d-flex flex-grow-1" + (search ? "" : " d-none")}>
                        <form action="/" role="search" className="flex-grow-1">
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
                        <form action="/" role="search" className="d-none d-sm-block position-absolute center col-sm-5">
                            <div className="d-flex gap-2 form-control py-0">
                                <input type="text" name="search" placeholder="Search" id="search-bar" className="form-control-plaintext" aria-label="Search" onChange={syncSearchInput}></input>
                                <button type="submit" className="btn bi bi-search"></button>
                            </div>
                        </form>
                        {email == undefined
                            ?
                            <div className="d-flex gap-2">
                                <a href="/account/login" className="btn btn-primary px-2">Login</a>
                                <a href="/account/register" className="d-none d-md-block btn btn-primary px-2">Register</a>
                            </div>
                            :
                            <div className="dropdown">
                                <button className="d-none d-lg-block btn btn-primary dropdown-toggle px-2" data-bs-toggle="dropdown" aria-expanded="false">{email}</button>
                                <button className="d-lg-none btn btn-primary bi bi-list" data-bs-toggle="dropdown" aria-expanded="false" />
                                <ul className="dropdown-menu dropdown-menu-end">
                                    <li key="profile"><span className="dropdown-item">{email}</span></li>
                                    <li key="logout"><button className="dropdown-item" onMouseDown={logout}>Logout</button></li>
                                </ul>
                            </div>
                        }
                    </div>
                </div>
            </nav>
        </header>
    );
};

export default navbar