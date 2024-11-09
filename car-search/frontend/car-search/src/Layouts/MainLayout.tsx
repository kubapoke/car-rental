import Navbar from "../Components/Navbar.tsx";
import { Outlet } from "react-router-dom";
import 'react-toastify/dist/ReactToastify.css';
import {ToastContainer} from 'react-toastify';

const MainLayout = () => {
    return (
        <>
            <Navbar/>
            <Outlet/>
            <ToastContainer/>
        </>
    );
};

export default MainLayout;