import {NavLink} from "react-router-dom";
import LogOutButton from "./LogOutButton.tsx";
import {FaRedhat} from "react-icons/fa";

const Navbar = () => {
    return (
        <nav className="bg-black border-b border-indigo-950">
            <div className="mx-auto max-w-7xl px-2 sm:px-6 lg:px-8">
                <div className="flex h-20 items-center justify-between">
                    <div className="flex flex-1 items-center justify-center md:items-stretch md:justify-start">
                        {/*<!-- Logo -->*/}
                        <NavLink className="flex flex-shrink-0 items-center mr-4" to="/">
                            <FaRedhat
                                className="h-10 w-auto text-white"
                            />
                            <span className="hidden md:block text-white text-2xl font-bold ml-2">
                                Top Secret Car Rental
                            </span>
                        </NavLink>
                        <div className="md:ml-auto">
                            <div className="flex space-x-2">
                                <LogOutButton/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </nav>
    );
};

export default Navbar;