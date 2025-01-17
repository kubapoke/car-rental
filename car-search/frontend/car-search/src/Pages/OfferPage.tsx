import {Link, useNavigate, useParams} from "react-router-dom";
import {FaArrowLeft, FaMapMarker} from "react-icons/fa";
import notFoundPage from "./ErrorPage.tsx";
import {useOffers, Offer} from "../Context/OffersContext.tsx";
import {calculateDaysBetweenDates} from "../Components/CarOffer.tsx";
import {useEffect} from "react";

const OfferPage = () => {
    const navigate = useNavigate();
    const { id } = useParams();

    // get offers from the context
    const { offers } = useOffers();

    useEffect(() => {
        if (!id) {
            navigate("/error", { state: { message: "Offer ID is missing", status: 400 } });
        }
    }, [id, navigate]);

    // find and save appropriate offer
    let offer: Offer | null = null;

    offer = offers.find((offer) => offer.offerId === id) || null;

    if (!offer) {
        const offerData = sessionStorage.getItem("offerData");

        if(offerData) {
            offer = JSON.parse(offerData);
        }
    }

    sessionStorage.setItem("offerData", JSON.stringify(offer));
    
    const onRentClick = async (offer: Offer) => {
        // rentCar(offer);
        const token = sessionStorage.getItem('authToken');
        const response = await fetch(`${import.meta.env.VITE_SERVER_URL}/api/Emails/send-email`, {
            method: 'POST',
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`, // this is required when we use [Authorize] api's
            },
            body: JSON.stringify(offer)
        });
        if (!response.ok) {
            console.log("Rent failed");
            throw new Error('Failed to rent');
        } else {
            // you need to check email
            navigate('/offers/rent-confirmation');
        }

    }

    if (!offer) {
        return (
            notFoundPage()
        );
    }
    
    return (
        <>
            <section>
                <div className="container m-auto py-6 px-6">
                    <Link
                        to="/offers"
                        className="text-indigo-500 hover:text-indigo-600 flex items-center"
                    >
                        <FaArrowLeft className="mr-2"></FaArrowLeft> Back to Offers
                    </Link>
                </div>
            </section>

            <section className="bg-indigo-50">
                <div className="container m-auto py-10 px-6">
                    <div className="grid grid-cols-1 md:grid-cols-70/30 w-full gap-6">
                        <main>
                            <div
                                className="bg-white p-6 rounded-lg shadow-md text-center md:text-left"
                            >
                                <h1 className="text-3xl font-bold mb-4">
                                    {offer.brand} {offer.model}
                                </h1>
                                <div className="text-orange-700 mb-4 flex align-left justify-left md:justify-start">
                                    <FaMapMarker className="mr-2"/>
                                    <p>{offer.location}</p>
                                </div>
                            </div>

                            <div className="bg-white p-6 rounded-lg shadow-md mt-6">
                                <h3 className="text-indigo-800 text-lg font-bold mb-6">
                                    Details
                                </h3>

                                <p className="mb-4">
                                    {offer.companyName} to polska firma rodzinna, która z pokolenia na pokolenie dostarcza najwyższej jakości usługi.
                                </p>

                                <h3 className="text-indigo-800 text-lg font-bold mb-2">Price</h3>

                                <p className="mb-4">{offer.price} PLN Total, {Math.round(offer.price /calculateDaysBetweenDates(offer.endDate, offer.startDate))} PLN / Day</p>
                            </div>
                        </main>

                        <div className="bg-white p-6 rounded-lg shadow-md mt-6">
                            <h3 className="text-xl font-bold mb-6">Actions</h3>
                            <button onClick={() => onRentClick(offer)}
                                    className="bg-red-500 hover:bg-red-600 text-white font-bold py-2 px-4 rounded-full max-w-lg focus:outline-none focus:shadow-outline mt-4 block"
                            >
                                Rent this car
                            </button>
                        </div>
                        
                    </div>
                </div>
            </section>
        </>
    );
};

export {OfferPage as default};