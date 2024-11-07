const navbar = () => {
    return (
        <header>
            <nav className="navbar navbar-expand-sm border-bottom border-2">
                <div className="container">
                    <a className="navbar-brand" href="/">
                        CodeReview
                    </a>
                    <form style={{ 'position': "absolute", 'left': "50%", 'width': "20rem", 'marginLeft': "-10rem" }}>
                        <input type="text" placeholder="Filter" className="form-control form-text"></input>
                    </form>
                    <a className="nav-link" href="/account/login">
                        Username
                    </a>
                </div>
            </nav>
        </header>
    );
};

export default navbar