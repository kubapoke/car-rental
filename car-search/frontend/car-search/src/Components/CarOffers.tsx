import {useEffect} from "react";
import CarOffer from './CarOffer.tsx'
import {useOffers} from "../Context/OffersContext.tsx";
import {useFilters} from "../Context/FiltersContext.tsx";

const CarOffers = ({isHome} : {isHome : boolean}) => {
    const {offers, setOffers} = useOffers();
    const filters = useFilters().filters;

    useEffect(() => {
        const fetchOffers = async () => {
            // Retrieve the authToken from session storage
            const token = sessionStorage.getItem('authToken'); // or use sessionStorage.getItem('jwt_token')

            // If the token is present, add it to the Authorization header
            const headers = {
                'Content-Type': 'application/json',
                ...(token && { 'Authorization': `Bearer ${token}` }), // Add the Authorization header if the token exists
            };

            const selectedBrand = filters.selectedBrand;
            const selectedModel = filters.selectedModel;
            const selectedLocation = filters.selectedLocation;
            const startDate = filters.startDate;
            const endDate = filters.endDate;

            const queryParams = new URLSearchParams();

            if (selectedBrand) queryParams.append('brand', selectedBrand);
            if (selectedModel) queryParams.append('model', selectedModel);
            if (selectedLocation) queryParams.append('location', selectedLocation);
            if (startDate) queryParams.append('startDate', startDate);
            if (endDate) queryParams.append('endDate', endDate);

            const apiUrl = `${import.meta.env.VITE_SERVER_URL}/api/OffersForward/offer-list?${queryParams.toString()}`;

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
    }, [filters, setOffers]);

    // Limit offers to 3 if isHome is true
    const displayedOffers = isHome ? offers.slice(0, 3) : offers;

    const getHeader = () =>
    {
        if(isHome){
            if(offers.length > 0)
                return "Recently viewed:";
            else
                return "Tell us what you are looking for";
        }
        else{
            if(offers.length > 0)
                return "Search results:";
            else
                return "No offers matching your requirements have been found :(";
        }
    }
    
    return (
        <section className="bg-blue-50 px-4 py-10">
            <div className="container-xl lg:container m-auto">
                <h2 className="text-3xl font-bold text-indigo-500 mb-6 text-center">
                    {getHeader()}
                </h2>
                    <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                        {displayedOffers.map((offer) => (
                            <CarOffer key={offer.carId} offer={offer}/>
                        ))}
                    </div>
            </div>
        </section>
    );
};

export default CarOffers;