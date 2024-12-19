import React, {useState} from "react";
import {FaLock} from "react-icons/fa";
import LogInForm from "../Components/LogInForm.tsx";

const LockComponent = () => {
    return (
        <div className="flex items-center justify-center">
            <span className="text-3xl text-white fa-solid"> <FaLock /> </span>
        </div>
    )
}

const LogInPage: React.FC = () => {
    const [wantsToLogIn, setWantsToLogIn] = useState(false);
    
    return (
        <div className="grid grid-cols-3 grid-rows-3 min-h-screen bg-black">
            <LockComponent/>
            <LockComponent/>
            <LockComponent/>
            <LockComponent/>


            {wantsToLogIn ? (
                <LogInForm />
            ) : (
                <div className="flex items-center justify-center">
                    <button
                        onClick={() => {
                           setWantsToLogIn(true)
                            console.log(wantsToLogIn)
                        }}
                        className="bg-white text-black py-2 px-4 rounded"
                    >
                        Log In
                    </button>
                </div>
            ) }

            <LockComponent/>
            <LockComponent/>
            <LockComponent/>
            <LockComponent/>
        </div>
    )
}

export default LogInPage;