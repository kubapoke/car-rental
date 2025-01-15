import {Link, useSearchParams} from "react-router-dom";
import {FaCheck, FaSkullCrossbones} from "react-icons/fa";
import {Fragment} from "react";

const RentConfirmedPage = () => {
    const [searchParams] = useSearchParams();
    const status = searchParams.get('status');
    const message = searchParams.get('message');
    return (
        <div>
            {status === 'success' ? (
                <>
                    <section className="text-center flex flex-col justify-center items-center h-96">
                    <FaCheck className={"fas text-yellow-400 text-6xl mb-4"}/>
                    <h1 className="text-6xl font-bold mb-4">Confirmation Successful</h1>
                    <p className="text-xl mb-5">Your rent has been successfully confirmed. We will take it from here.</p>
                    <p className="text-xl mb-5">Thank you for choosing Rent a Car! Your car is on the way!</p>
                    <Link
                        to="/"
                        className="text-white bg-blue-700 hover:bg-blue-900 rounded-md px-3 py-2 mt-4"
                    >Go Back</Link
                    >
                    </section> 
                </>
                ) : (
                    <>
                <section className="text-center flex flex-col justify-center items-center h-96">
                    <FaSkullCrossbones className={"fas text-yellow-400 text-6xl mb-4"}/>
                    <h1 className="text-6xl font-bold mb-4">Something went wrong</h1>
                    <p className="text-xl mb-5">We have encountered an error... Sorry!</p>
                    <p className="text-xl mb-5">Error message: {message}</p>
                    <Link
                        to="/"
                        className="text-white bg-blue-700 hover:bg-blue-900 rounded-md px-3 py-2 mt-4"
                    >Go Back</Link
                    >
                </section>
                </>
            )}
        </div>
    );
};

export default RentConfirmedPage;