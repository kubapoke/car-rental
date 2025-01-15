import {useSearchParams} from "react-router-dom";

const RentConfirmedPage = () => {
    const [searchParams] = useSearchParams();
    const status = searchParams.get('status');
    const message = searchParams.get('message');
    return (
        <div>
            {status === 'success' ? (
                <h1>Confirmation Successful!</h1>
            ) : (
                <h1>Error: {message}</h1>
            )}
        </div>
    );
};

export default RentConfirmedPage;