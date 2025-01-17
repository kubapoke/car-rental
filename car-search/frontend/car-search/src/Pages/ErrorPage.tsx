import { Link, useLocation } from 'react-router-dom';
import { FaExclamationTriangle } from "react-icons/fa";

const ErrorPage = () => {
    const location = useLocation();
    const { message, status } = location.state || {};

    return (
        <section className="text-center flex flex-col justify-center items-center h-96">
            <FaExclamationTriangle className="fas text-yellow-400 text-6xl mb-4" />
            <h1 className="text-6xl font-bold mb-4">
                Error {status || 404}
            </h1>
            <p className="text-xl mb-5">
                {message || "This page does not exist"}
            </p>
            <Link
                to="/"
                className="text-white bg-blue-700 hover:bg-blue-900 rounded-md px-3 py-2 mt-4"
            >
                Go Back
            </Link>
        </section>
    );
};

export default ErrorPage;
