import React, { createContext, useState, useContext, ReactNode } from 'react';

// this is a context file, created so that the search results persist between pages

interface Filters {
    selectedBrand: string;
    selectedModel: string;
    selectedLocation: string;
    startDate: string;
    endDate: string;
}

interface FiltersContextValue {
    filters: Filters;
    setFilters: React.Dispatch<React.SetStateAction<Filters>>;
}

const FiltersContext = createContext<FiltersContextValue | undefined>(undefined);

export const FiltersProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [filters, setFilters] = useState<Filters>({
        selectedBrand: '',
        selectedModel: '',
        selectedLocation: '',
        startDate: '',
        endDate: '',
    });

    return (
        <FiltersContext.Provider value={{ filters, setFilters }}>
            {children}
        </FiltersContext.Provider>
    );
};

export const useFilters = () => {
    const context = useContext(FiltersContext);
    if (!context) {
        throw new Error('useFilters must be used within a FiltersProvider');
    }
    return context;
};