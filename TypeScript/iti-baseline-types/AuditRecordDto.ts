export interface AuditRecordDto {
    id: number
    whenUtc: Date
    userId: string
    userName: string
    aggregate: string
    aggregateId: string
    entity: string
    entityId: string
    event: string
    changes: string
}
