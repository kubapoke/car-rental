import {useState, useEffect} from "react";
import CarOffer from './CarOffer.tsx'

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

const CarOffers = () => {
    const [offers, setOffers] = useState<Offer[]>([]);

    useEffect(() => {
        const fetchOffers = async () => {
            // Retrieve the authToken from session storage
            const token = sessionStorage.getItem('authToken'); // or use sessionStorage.getItem('jwt_token')

            // If the token is present, add it to the Authorization header
            const headers = {
                'Content-Type': 'application/json',
                ...(token && { 'Authorization': `Bearer ${token}` }), // Add the Authorization header if the token exists
            };

            const apiUrl = `${import.meta.env.VITE_SERVER_URL}/api/OffersForward/offer-list?startDate=2025-12-18&endDate=2025-12-19`

            try {
                const res = await fetch(apiUrl, {method: 'GET', headers: headers});
                const data = await res.json();
                setOffers(data);
            }
            catch (error){
                console.log('Error fetching data', error);
            }
        }

        fetchOffers();
    }, []);
    
    return (
        <section className="bg-blue-50 px-4 py-10">
            <div className="container-xl lg:container m-auto">
                <h2 className="text-3xl font-bold text-indigo-500 mb-6 text-center">
                    You might like:
                </h2>
                    <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                        {offers.map((offer) => (
                            <CarOffer key={offer.carId} offer={offer}/>
                        ))}
                    </div>
            </div>
        </section>
    );
};

export default CarOffers;