import logo from '../Assets/Images/logo.png';
import {NavLink} from 'react-router-dom';

const Navbar = () => {
    return (
        <>
            <nav className="bg-blue-700 border-b border-blue-500">
                <div className="mx-auto max-w-7xl px-2 sm:px-6 lg:px-8">
                    <div className="flex h-20 items-center justify-between">
                        <div
                            className="flex flex-1 items-center justify-center md:items-stretch md:justify-start"
                        >
                            {/*<!-- Logo -->*/}
                            <NavLink className="flex flex-shrink-0 items-center mr-4" to="/">
                                <img
                                    className="h-10 w-auto"
                                    src={logo}
                                    alt="React logo"
                                />
                                <span className="hidden md:block text-white text-2xl font-bold ml-2"
                                >Rent a Car</span
                                >
                            </NavLink>
                            <div className="md:ml-auto">
                                <div className="flex space-x-2">
                                    <NavLink
                                        to="/login"
                                        className='text-white hover:bg-gray-900 hover:text-white rounded-md px-3 py-2'
                                    >Log in</NavLink
                                    >
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </nav>
        </>
    );
};

export default Navbar;