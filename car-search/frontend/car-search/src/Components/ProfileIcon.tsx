import {FaUser} from "react-icons/fa";
import React, {useEffect, useRef} from "react";
import {useState} from "react";
import {useNavigate} from "react-router-dom";
import {useOffers} from "../Context/OffersContext.tsx";


interface ProfileIconProps
{
    email: string,
    logout: () => void;
}


const ProfileIcon:React.FC<ProfileIconProps> = ({email, logout} : {email : string, logout: () => void} ) => {
    const navigate = useNavigate();
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    // this allows checking if the user clicked somewhere else
    const menuRef = useRef<HTMLDivElement>(null);
    const toggleMenu = () => {
        setIsMenuOpen(!isMenuOpen);
    };
    const {setOffers} = useOffers();

    const closeMenu = () => {
        setIsMenuOpen(false);
    };

    const handleRentsClick = () => {
        setIsMenuOpen(false);
        navigate("/user-rents");
    };

    const handleLogoutClick = () => {
        setIsMenuOpen(false);
        setOffers([]);
        logout();
    };

    useEffect(() => {
        const handleOutsideClick = (event: MouseEvent) => {
            if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
                closeMenu();
            }
        };

        document.addEventListener("mousedown", handleOutsideClick);
        return () => {
            document.removeEventListener("mousedown", handleOutsideClick);
        };
    }, []);
    
    return (
    <div className="relative flex flex-col items-center justify-center h-screen text-center">
        <FaUser className="text-2xl text-white mt-4" onClick={toggleMenu}/>
        <p className="mt-2 text-lg text-white" onClick={toggleMenu}>{email}</p>

        {isMenuOpen && (
            <div ref={menuRef} className="absolute mt-44 bg-white border rounded-lg shadow-lg w-48 z-3">
                <ul className="flex flex-col">
                    <li
                        className="p-2 cursor-pointer hover:bg-gray-200"
                        onClick={handleRentsClick}
                    >
                        Your rents
                    </li>
                    <li
                        className="p-2 cursor-pointer hover:bg-gray-200"
                        onClick={handleLogoutClick}
                    >
                        Logout
                    </li>
                </ul>
            </div>
        )}
    </div>
    );
};

export default ProfileIcon;