import {Link} from "react-router-dom";
import {FaMapMarker} from "react-icons/fa";
import {Offer} from "../Context/OffersContext.tsx";

const CarOffer = ({offer} : {offer : Offer}) => {
    return (
        <div className="bg-white rounded-xl shadow-md relative">
            <div className="p-4">
                <div className="mb-6">

                    <h3 className="text-xl font-bold">{offer.brand} {offer.model}</h3>
                </div>

                <div className="text-orange-700 mb-4 flex align-start justify-start md:justify-start">
                    <FaMapMarker className="mr-2"/>
                    <p>{offer.location}</p>
                </div>

                <h3 className="text-indigo-500 mb-2">{offer.price} PLN Total, {Math.round(offer.price /calculateDaysBetweenDates(offer.endDate, offer.startDate))} PLN / Day</h3>


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

export const calculateDaysBetweenDates = (date1: string, date2: string): number => {
    const firstDate = new Date(date1);
    const secondDate = new Date(date2);

    // Obliczenie różnicy czasu w milisekundach
    const differenceInMilliseconds = Math.abs(secondDate.getTime() - firstDate.getTime());

    // Przeliczenie na dni
    const differenceInDays = differenceInMilliseconds / (1000 * 60 * 60 * 24);

    return Math.round(differenceInDays) + 1;
};

export default CarOffer;