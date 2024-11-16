import {Link} from "react-router-dom";

interface Offer {
    carId: number;
    brand: string;
    model: string;
    email: string;
    price: number;
    conditions: string;
    companyName: string;
    startDate: string;
    endDate: string;
}

const CarOffer = ({offer} : {offer : Offer}) => {
    return (
        <div className="bg-white rounded-xl shadow-md relative">
            <div className="p-4">
                <div className="mb-6">
                    <h3 className="text-xl font-bold">{offer.brand} {offer.model}</h3>
                </div>

                <h3 className="text-indigo-500 mb-2">{offer.price} / Day</h3>

                <div className="border border-gray-100 mb-5"></div>

                <div className="flex flex-col lg:flex-row justify-between mb-4">
                    <Link
                        to={`/offers/${offer.carId}`}
                        className="h-[36px] bg-indigo-500 hover:bg-indigo-600 text-white px-4 py-2 rounded-lg text-center text-sm"
                    >
                        Read More
                    </Link>
                </div>
            </div>
        </div>
    );
};

export default CarOffer;