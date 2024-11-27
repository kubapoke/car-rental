import { Outlet } from "react-router-dom";
import Navbar from "../Components/Navbar.tsx";

const MainLayout = () => {
    return (
        <>
            <Navbar/>
            <Outlet/>
        </>
    );
};

export default MainLayout;