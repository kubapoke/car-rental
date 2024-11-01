import Navbar from "../Components/Navbar.tsx";
import { Outlet } from "react-router-dom";

const MainLayout = () => {
    return (
        <>
            <Navbar/>
            <Outlet/>
        </>
    );
};

export default MainLayout;