import {Link, useParams} from "react-router-dom";
import {FaArrowLeft, FaMapMarker} from "react-icons/fa";
import notFoundPage from "./NotFoundPage.tsx";
import {useOffers} from "../Context/OffersContext.tsx";

interface OfferPageProps {
    rentCar: (offerId: number) => void;
}

const OfferPage = ({rentCar}: OfferPageProps) => {
    const { id } = useParams();
    if (!id) {
        throw new Response("Offer ID is missing", { status: 400 });
    }

    // get offers from the context
    const { offers } = useOffers();

    // find appropriate offer
    const offer = offers.find((offer) => offer.carId === Number(id));

    if (!offer) {
        throw new Response("Offer not found", { status: 404 });
    }
    
    const onRentClick = (offerId: number) => {
        rentCar(offerId);
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
                                <div
                                    className="text-gray-500 mb-4 flex align-middle justify-center md:justify-start"
                                >
                                    <FaMapMarker
                                        className="mr-2"/>
                                    <p className="text-orange-700">Warsaw</p>
                                </div>
                            </div>

                            <div className="bg-white p-6 rounded-lg shadow-md mt-6">
                                <h3 className="text-indigo-800 text-lg font-bold mb-6">
                                    Details
                                </h3>

                                <p className="mb-4">
                                    bla bla bla bla bla bla bla bla bla bla
                                </p>

                                <h3 className="text-indigo-800 text-lg font-bold mb-2">Price</h3>

                                <p className="mb-4">{offer.price}/ Day</p>
                            </div>
                        </main>

                        <div className="bg-white p-6 rounded-lg shadow-md mt-6">
                            <h3 className="text-xl font-bold mb-6">Actions</h3>
                            <button onClick={() => onRentClick(offer.carId)}
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