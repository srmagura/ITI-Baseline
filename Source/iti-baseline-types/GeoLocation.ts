export const GeoLocationTypeName = 'GeoLocation'
export interface GeoLocation {
    source: string
    longitude: number | null
    latitude: number | null
    isValid: boolean
    isConfident: boolean
    status: string
    locationType: string
    formattedAddress: string
}
