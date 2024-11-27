import { Outlet } from "react-router-dom";
import Navbar from "../Components/Navbar.tsx";

const LoggedInLayout = () => {
    return (
        <>
            <Navbar/>
            <Outlet/>
        </>
    );
};

export default LoggedInLayout;